using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FinalTask.Domain.Abstract;
using FinalTask.Tests.Concrete;
using Ninject;

namespace FinalTask.Tests.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<ITaskRepository>().To<MockRepository>();
        }
    }
}
