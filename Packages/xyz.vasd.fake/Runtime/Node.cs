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
        public ITask[] Dependencies;

        public Task(params ITask[] dependencies)
        {
            Dependencies = dependencies;
        }

        public bool Execute()
        {
            if (Dependencies != null && !Dependencies.All(task => task.Execute())) return false;
            return OnExecute();
        }

        public virtual bool OnExecute()
        {
            return true;
        }

        public Task Then(ITask task)
        {
            return new Task(this, task);
        }
    }

    public class QuickTask : ITask
    {
        public delegate bool Task();
        public delegate void TaskVoid();

        private Task _task;

        public QuickTask(Task task)
        {
            _task = task;
        }

        public QuickTask(TaskVoid task)
        {
            _task = VoidToTask(task);
        }

        public bool Execute()
        {
            return _task();
        }

        public QuickTask Then(Task task)
        {
            return new QuickTask(() => Execute() && task());
        }
        public QuickTask Then(TaskVoid task)
        {
            return new QuickTask(() => Execute() && VoidToTask(task)());
        }

        public static QuickTask Create(Task task)
        {
            return new QuickTask(task);
        }

        public static QuickTask Create(TaskVoid task)
        {
            return new QuickTask(task);
        }

        private static Task VoidToTask(TaskVoid task)
        {
            return () =>
            {
                task();
                return true;
            };
        }
    }

    public class Route : MonoBehaviour
    {
        public Animator Animator;

        private QuickTask _startTask;
        private QuickTask _stopTask;

        private void Awake()
        {
            _startTask = QuickTask
                .Create(() => Animator.Play(0))
                .Then(() => Animator.GetBool("started"));

            _stopTask = QuickTask
                .Create(() => Animator.SetBool("stop", true))
                .Then(() => Debug.Log("bool was set"))
                .Then(() => Animator.GetBool("stopped"));
        }

        public bool Open()
        {
            return _startTask.Execute();
        }

        public bool Close()
        {
            return _stopTask.Execute();
        }
    }
}
