using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnityEngine;

namespace Xyz.Vasd.Fake
{

    public interface ITask
    {
        bool Execute();
    }

    public class Task : ITask
    {
        public bool IsCompleted { get; private set; }

        public bool Execute()
        {
            if (IsCompleted) return true;

            if (OnExecute()) IsCompleted = true;
            return IsCompleted;
        }

        public virtual bool OnExecute()
        {
            return true;
        }
    }

    public class QuickTask : ITask
    {
        public delegate bool TaskFunction();
        public delegate void TaskFunctionVoid();

        public readonly TaskFunction Fn;

        public QuickTask(TaskFunction fn)
        {
            Fn = fn;
        }

        public QuickTask(TaskFunctionVoid fn)
        {
            Fn = () =>
            {
                fn();
                return true;
            };
        }

        public bool Execute()
        {
            return Fn();
        }

        public QuickTask Then(TaskFunction fn)
        {
            var nextTask = new QuickTask(fn);

            return new QuickTask(() =>
            {
                return Execute() && nextTask.Execute();
            });
        }

        public QuickTask Then(TaskFunctionVoid fn)
        {
            var nextTask = new QuickTask(fn);

            return new QuickTask(() =>
            {
                return Execute() && nextTask.Execute();
            });
        }

        public static QuickTask Create(TaskFunction fn)
        {
            return new QuickTask(fn);
        }

        public static QuickTask Create(TaskFunctionVoid fn)
        {
            return new QuickTask(fn);
        }
    }

    public class Page
    {
        public Animator Animator;

        public ITask StartTask;
        public ITask StopTask;

        public void Awake()
        {
            StartTask = QuickTask
                .Create(() => Animator.Play(0))
                .Then(() => Animator.GetBool("start_completed"));

            StopTask = QuickTask
                .Create(() => Animator.SetBool("stop", true))
                .Then(() => Animator.GetBool("stop_completed"));
        }

        public bool Start()
        {
            return StartTask.Execute();
        }

        public bool Stop()
        {
            return StopTask.Execute();
        }
    }

}
