
using UnityEngine;

public class Heart : MonoBehaviour
{
    private Transform[] _hearts;

    private void Awake()
    {
        _hearts = new Transform[3];

        for (int i = 0; i < _hearts.Length; i++)
            _hearts[i] = transform.GetChild(i);
        
    }

    public void DestroyHeart()
    {
        for (int i = 0; i < _hearts.Length; i++)
        {
//            if(i < Ball.CountHeart) 
//                _hearts[i].gameObject.SetActive(true);
//            else
//                _hearts[i].gameObject.SetActive(false);
        }
    }
}
