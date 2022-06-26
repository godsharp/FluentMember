using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace GodSharp.FluentMember;

internal sealed class MemberSetter
{
    private readonly ParameterExpression _body;
    private readonly ConcurrentDictionary<string, object> _handles;
    private readonly ConcurrentDictionary<string, MemberExpression> _members;

    public MemberSetter(Type type)
    {
        _body = Expression.Parameter(type, $"_{type.Name}");
        _handles = new ConcurrentDictionary<string, object>();
        _members = new ConcurrentDictionary<string, MemberExpression>();

        foreach (var property in type
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.SetMethod?.IsPublic == true || x.SetMethod?.IsAssembly == true)
        )
        {
            _members.TryAdd(property.Name, Expression.Property(_body, property));
        }
        foreach (var field in type
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(x => x.IsPublic || x.IsAssembly))
        {
            _members.TryAdd(field.Name, Expression.Field(_body, field));
        }
    }

    public void Set<TSource, TValue>(TSource source, TValue value, string member)
        where TSource : class
    {
        if (_handles.ContainsKey(member))
        {
            (_handles[member] as Action<TSource, TValue>)?.Invoke(source, value);
        }
        else
        {
            var handle = Create<TSource, TValue>(member);
            _handles.TryAdd(member, handle);
            handle?.Invoke(source, value);
        }
    }

    private Action<TSource, TValue> Create<TSource, TValue>(string member)
        where TSource : class
    {
        if (!_members.TryGetValue(member, out var mb) || mb == null)
        {
            throw new MemberAccessException($"not accessible member {member} of type {typeof(TSource).Name}.");
        }
        var parameter = Expression.Parameter(mb.Type, member.ToLower());
        var assign = Expression.Assign(mb, parameter);

        return Expression.Lambda<Action<TSource, TValue>>(assign, _body, parameter).Compile();
    }
}