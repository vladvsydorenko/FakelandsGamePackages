﻿using UnityEngine;

namespace Xyz.Vasd.Fake.Task
{
    public class Page
    {
        private IFakeTask _openTask;
        private IFakeTask _closeTask;

        private Animator Animator;

        public void Setup()
        {
            _openTask = FakeTask
                .Function((v) =>
                {
                    Animator.Play(0);
                })
                .Then((v) =>
                {
                    return Animator.GetBool("open");
                });

            //_openTask = FakeTaskLambda2
            //    .Create(() =>
            //    {
            //        Animator.Play(0);
            //    })
            //    .Then(() =>
            //    {
            //        return Animator.GetBool("open");
            //    });

            //_closeTask = FakeTaskLambda2
            //    .Create(() =>
            //    {
            //        Animator.SetBool("close", true);
            //    })
            //    .Then(() =>
            //    {
            //        return Animator.GetBool("closed");
            //    });
        }

        public bool Open()
        {
            return _openTask.Execute(version: 0);
        }

        public bool Close()
        {
            return _closeTask.Execute(version: 0);
        }
    }
}