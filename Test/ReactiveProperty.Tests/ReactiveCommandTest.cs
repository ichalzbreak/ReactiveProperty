﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reactive.Bindings;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using System.Reactive.Subjects;
using System.Threading;

namespace ReactiveProperty.Tests
{
    [TestClass]
    public class ReactiveCommandTest : ReactiveTest
    {
        [TestMethod]
        public void ReactiveCommandAllFlow()
        {
            var testScheduler = new TestScheduler();
            var @null = (object)null;
            var recorder1 = testScheduler.CreateObserver<object>();
            var recorder2 = testScheduler.CreateObserver<object>();

            var cmd = new ReactiveCommand();
            cmd.Subscribe(recorder1);
            cmd.Subscribe(recorder2);

            cmd.CanExecute().Is(true);
            cmd.Execute(); testScheduler.AdvanceBy(10);
            cmd.Execute(); testScheduler.AdvanceBy(10);
            cmd.Execute(); testScheduler.AdvanceBy(10);

            cmd.Dispose();
            cmd.CanExecute().Is(false);

            cmd.Dispose(); // dispose again

            recorder1.Messages.Is(
                OnNext(0, @null),
                OnNext(10, @null),
                OnNext(20, @null),
                OnCompleted<object>(30));

            recorder2.Messages.Is(
                OnNext(0, @null),
                OnNext(10, @null),
                OnNext(20, @null),
                OnCompleted<object>(30));
        }

        [TestMethod]
        public void ReactiveCommandSubscribe()
        {
            var testScheduler = new TestScheduler();
            var recorder1 = testScheduler.CreateObserver<int>();
            var recorder2 = testScheduler.CreateObserver<int>();

            var cmd = new ReactiveCommand();
            int counter = 0;
            Action countUp = () => counter++;
            cmd.Subscribe(countUp);
            Action recordAction1 = () => recorder1.OnNext(counter);
            cmd.Subscribe(recordAction1);
            Action recordAction2 = () => recorder2.OnNext(counter);
            cmd.Subscribe(recordAction2);

            cmd.Execute(); testScheduler.AdvanceBy(10);
            cmd.Execute(); testScheduler.AdvanceBy(10);
            cmd.Execute(); testScheduler.AdvanceBy(10);

            recorder1.Messages.Is(
                OnNext(0, 1),
                OnNext(10, 2),
                OnNext(20, 3));
            recorder2.Messages.Is(
                OnNext(0, 1),
                OnNext(10, 2),
                OnNext(20, 3));
        }
    }
}
