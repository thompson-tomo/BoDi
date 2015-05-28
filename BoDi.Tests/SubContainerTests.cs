﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace BoDi.Tests
{
    [TestFixture]
    public class SubContainerTests
    {
        [Test]
        public void ShouldBeAbleToResolveFromBaseContainer()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<VerySimpleClass, IInterface1>();
            var container = new ObjectContainer(baseContainer);

            // when

            var obj = container.Resolve<IInterface1>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(VerySimpleClass), obj);
        }

        [Test]
        public void ShouldBeAbleToResolveFromChildContainer()
        {
            // given
            var baseContainer = new ObjectContainer();
            var container = new ObjectContainer(baseContainer);
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // when

            var obj = container.Resolve<IInterface1>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(VerySimpleClass), obj);
        }

        [Test]
        public void ShouldResolveFromBaseContainer()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<VerySimpleClass, IInterface1>();
            var container = new ObjectContainer(baseContainer);

            // when
            var objFromChild = container.Resolve<IInterface1>();
            var objFromBase = baseContainer.Resolve<IInterface1>();

            // then
            Assert.AreSame(objFromChild, objFromBase);
        }

        [Test]
        public void ShouldBeAbleToOverrideBaseContainerRegistration()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<SimpleClassWithDefaultCtor, IInterface1>();
            var container = new ObjectContainer(baseContainer);
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // when

            var obj = container.Resolve<IInterface1>();
            var baseObj = baseContainer.Resolve<IInterface1>();

            // then

            Assert.IsNotNull(obj);
            Assert.IsInstanceOf(typeof(VerySimpleClass), obj);
            Assert.AreNotEqual(obj, baseObj);
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void BaseContainerMustBeAnObjectContainer()
        {
            var otherContainer = new Mock<IObjectContainer>();

            var container = new ObjectContainer(otherContainer.Object);
        }

        [Test]
        public void ParentObjectPoolShouldNotBeConsideredWhenReRegisteredInChild()
        {
            // given
            var baseContainer = new ObjectContainer();
            baseContainer.RegisterTypeAs<VerySimpleClass, IInterface1>();
            var container = new ObjectContainer(baseContainer);
            container.RegisterTypeAs<VerySimpleClass, IInterface1>();

            // when
            var objFromBase = baseContainer.Resolve<IInterface1>();
            var objFromChild = container.Resolve<IInterface1>();

            // then
            Assert.AreNotSame(objFromChild, objFromBase);
        }
    }
}
