using System;
using UnityEngine;

namespace Code.Core.Managers
{
    public class CreateOnceManager : MonoBehaviour
    {
        private void Awake()
        {
            var isAlreadyExists = FindAnyObjectByType<CreateOnceManager>();

            //찾은 녀석이 있고 그게 나랑 다른 녀석이라면 나를 제거한다.
            if (isAlreadyExists != null && isAlreadyExists != this)
            {
                Destroy(gameObject);
                return;
            }
            
            DontDestroyOnLoad(gameObject); //그렇지 않다면 나를 뽀개지 못하게 한다.
            
            //이렇게 하면 처음 로딩된 애가 가장 우선순위를 가지고 나중에 배치된 녀석들은 자동으로 파괴된다.
        }
    }
}