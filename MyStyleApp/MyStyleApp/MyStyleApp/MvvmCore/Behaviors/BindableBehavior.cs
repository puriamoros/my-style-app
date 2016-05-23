﻿using System;
using Xamarin.Forms;

namespace MvvmCore.Behaviors
{
    /// <summary>
    /// This base behavior class is aware of the binding context of its associated view,
    /// so that when the view's binding context changes, the behavior binding context will be the same.
    /// </summary>
    /// <typeparam name="T">The type of the objects with which this <see cref="Behavior"/> can be associated.</typeparam>
    public class BindableBehavior<T> : Behavior<T> where T : BindableObject
    {
        public T AssociatedObject { get; private set; }

        protected override void OnAttachedTo(T visualElement)
        {
            base.OnAttachedTo(visualElement);

            AssociatedObject = visualElement;

            if (visualElement.BindingContext != null)
                BindingContext = visualElement.BindingContext;

            visualElement.BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

        protected override void OnDetachingFrom(T view)
        {
            view.BindingContextChanged -= OnBindingContextChanged;
        }

        /// <summary>
        /// Track any changes of the view's binding context
        /// </summary>
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }
    }
}
