// Copyright 2004 DigitalCraftsmen - http://www.digitalcraftsmen.com.br/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.DynamicProxy
{
	using System.Reflection;

	/// <summary>
	/// 
	/// </summary>
	public interface ICallable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		object Call(params object[] args);
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IInvocation
	{
		/// <summary>
		/// 
		/// </summary>
		object Proxy { get; }

		/// <summary>
		/// 
		/// </summary>
		object InvocationTarget { get; }

		/// <summary>
		/// 
		/// </summary>
		MethodInfo Method { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		object Proceed( params object[] args );
	}

	public abstract class AbstractInvocation : IInvocation
	{
		private object proxy;
		private object target;
		private MethodInfo method;

		public AbstractInvocation( object proxy, object target, MethodInfo method )
		{
			this.proxy = proxy;
			this.target = target;
			this.method = method;
		}

		public object Proxy
		{
			get { return proxy; }
		}

		public object InvocationTarget
		{
			get { return target; }
		}

		public MethodInfo Method
		{
			get { return method; }
		}

		public abstract object Proceed(params object[] args);
	}

	/// <summary>
	/// 
	/// </summary>
	public class InterfaceInvocation : AbstractInvocation
	{
		public InterfaceInvocation(object proxy, object target, MethodInfo method) : base(proxy, target, method)
		{
		}

		public override object Proceed(params object[] args)
		{
			return Method.Invoke(InvocationTarget, args);
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public class SameClassInvocation : AbstractInvocation
	{
		private ICallable call;

		public SameClassInvocation(ICallable del, object proxy, object target, MethodInfo method) : base(proxy, target, method)
		{
			call = del;
		}

		public override object Proceed(params object[] args)
		{
			object value = call.Call( args );
			return value;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IInterceptor
	{
		object Intercept( IInvocation invocation, params object[] args );
	}
}
