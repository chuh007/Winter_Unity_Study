using System;
using UnityEngine;

namespace Code.Core.Managers
{
    public class CreateOnceManager : MonoBehaviour
    {
        private void Awake()
        {
            var isAlreadyExists = FindAnyObjectByType<CreateOnceManager>();

            // 찾은 녀석이 있고, 그게 내가 아니면 나 제거
            if (isAlreadyExists != null && isAlreadyExists != this)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject); // 그렇지 않으면 파괴 불가하게
            // 이러면 처음 로딩된 애만 남고, 나머지는 다 지워짐
        }
    }
}