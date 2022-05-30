// using UnityEngine;
// using Sirenix.OdinInspector;

// [CreateAssetMenu(fileName = "RandomValueSetting", menuName = "Unity Template/RandomValueSetting", order = 0)]
// public class RandomValueSetting : ScriptableObject
// {
//     //Move two value when pressed CTRL + Left click

//     [MinMaxSlider(0, 2), SerializeField] Vector2 duration;
//     public float Duration => Random.Range(duration.x, duration.y);

//     [SerializeField]
//     [MinMaxSlider(-1, 1, true)]
//     Vector2[] position;
//     public Vector3 RndPosition => new Vector3(
//             Random.Range(position[0].x, position[0].y),
//             Random.Range(position[1].x, position[1].y),
//             Random.Range(position[2].x, position[2].y));

//     [SerializeField]
//     [MinMaxSlider(-1, 1, true)]
//     Vector2[] scale;
//     public Vector3 RndScale => new Vector3(
//            Random.Range(scale[0].x, scale[0].y),
//            Random.Range(scale[1].x, scale[1].y),
//            Random.Range(scale[2].x, scale[2].y));

//     [SerializeField]
//     [MinMaxSlider(-1, 1, true)]
//     Vector2[] rotation;
//     public Vector3 Rotation => new Vector3(
//            Random.Range(rotation[0].x, rotation[0].y),
//            Random.Range(rotation[1].x, rotation[1].y),
//            Random.Range(rotation[2].x, rotation[2].y));

//     public Vector3 GetRndRot(int k) => new Vector3(Random.value, Random.value, Random.value) * k;
//     public Quaternion GetRndRotQuaternion(int k) => Quaternion.Euler(new Vector3(Random.value, Random.value, Random.value) * k);

//     public AnimationCurve animationCurve;
// }