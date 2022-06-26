using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace GodSharp.FluentMember;

public sealed class TypeSetter
{
    private static readonly ConcurrentDictionary<Type, MemberSetter> _setters;

    static TypeSetter()
    {
        _setters = new ConcurrentDictionary<Type, MemberSetter>();
    }

    private static TSource Set<TSource, TMember>(TSource source, TMember value, string member)
        where TSource : class
    {
        var type = typeof(TSource);
        if (_setters.ContainsKey(type))
        {
            _setters[type].Set(source, value, member);
        }
        else
        {
            var setter = new MemberSetter(type);
            _setters.TryAdd(type, setter);
            setter.Set(source, value, member);
        }

        return source;
    }

    public static TSource Set<TSource, TMember>(TSource source, Expression<Func<TSource, TMember>> predicate, TMember value)
        where TSource : class
    {
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return Set(source, value, GetMemberName(predicate.Body));
    }

    private static string GetMemberName(Expression expression)
    {
        var name = expression switch
        {
            UnaryExpression ue => ((MemberExpression)ue.Operand).Member.Name,
            MemberExpression me => me.Member.Name,
            ParameterExpression pe => pe.Type.Name,
            _ => throw new InvalidCastException("Only field and property can set.")
        };

        return name;
    }
}