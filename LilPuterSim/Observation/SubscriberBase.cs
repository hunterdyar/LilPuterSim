using System;
using System.Collections.Generic;

namespace LilPuter
{
	public abstract class SubscriberBase<T> : IObservable<T>
	{
		protected List<IObservable<T>.OnValueChangeDelegate> _subscribers = new List<IObservable<T>.OnValueChangeDelegate>();
		public void Subscribe(IObservable<T>.OnValueChangeDelegate subscriber)
		{
			if(!_subscribers.Contains(subscriber))
			{
				_subscribers.Add(subscriber);
			}
			else
			{
				throw new Exception("Subscriber already exists. Can't add.");
			}
		}

		public void Unubscribe(IObservable<T>.OnValueChangeDelegate subscriber)
		{
			if (!_subscribers.Remove(subscriber))
			{
				throw new Exception("Subscriber not found. Can't Remove.");
			}
		}

		protected void UpdateSubscribers()
		{
			foreach (var onChange in _subscribers)
			{
				onChange(ReadValue());
			}
		}

		public int SubscriberCount()
		{
			return _subscribers.Count;
		}

		public abstract T ReadValue();
	}
}