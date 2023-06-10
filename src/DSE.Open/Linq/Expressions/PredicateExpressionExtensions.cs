// Copyright (c) Down Syndrome Education International and Contributors. All Rights Reserved.
// Down Syndrome Education International and Contributors licence this file to you under the MIT license.

using System.Linq.Expressions;

namespace DSE.Open.Linq.Expressions;

public static class PredicateExpressionExtensions
{
    public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        => Combine(left, right, Expression.And);

    /// <summary>
    /// Creates an expression that represents a conditional AND operation
    /// that evaluates the second operand only if the first operand evaluates to true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        => Combine(left, right, Expression.AndAlso);

    public static Expression<Func<T, bool>> AndAlsoIfNotNull<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>>? right)
    {
        Guard.IsNotNull(left);

        return right is null ? left : left.AndAlso(right);
    }

    public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        => Combine(left, right, Expression.Or);

    public static Expression<Func<T, bool>> Combine<T>(
        Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right,
        Func<Expression, Expression, BinaryExpression> combineOperator)
    {
        Guard.IsNotNull(left);
        Guard.IsNotNull(right);
        Guard.IsNotNull(combineOperator);

        var leftParameter = left.Parameters[0];
        var rightParameter = right.Parameters[0];

        var visitor = new ReplaceExpressionVisitor(rightParameter, leftParameter);

        var leftBody = left.Body;
        var rightBody = visitor.Visit(right.Body)!;

        return Expression.Lambda<Func<T, bool>>(combineOperator(leftBody, rightBody), leftParameter);
    }

    private class ReplaceExpressionVisitor
        : ExpressionVisitor
    {
        private readonly Expression _oldValue;
        private readonly Expression _newValue;

        public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
        {
            _oldValue = oldValue;
            _newValue = newValue;
        }

        public override Expression? Visit(Expression? node) => node == _oldValue ? _newValue : base.Visit(node);
    }
}
