using System.Collections;
using UnityEngine;

namespace Main.Scripts.Infrastructure.Bootstrap
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine routine);
    }
}