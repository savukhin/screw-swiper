using System.Collections.Generic;
using System.Linq;
using UnityEngine;


struct SwiperData {
    public Vector2 startPosition;
    public Vector2 lastPosition;

    public SwiperData(Vector2 startPosition_) {
        this.startPosition = startPosition_;
        this.lastPosition = startPosition_;
    } 

    public SwiperData(Vector2 startPosition_, Vector2 lastPosition_) {
        this.startPosition = startPosition_;
        this.lastPosition = lastPosition_;
    } 
}


struct MyTouch {
    public int id;
    public Vector2 position;

    public MyTouch(int id_, Vector2 position_) {
        id = id_;
        position = position_;
    }
}


public class SwipeDetector : MonoBehaviour
{
    // private Dictionary<int, Vector2> startPositions;
    private Dictionary<int, SwiperData> swipes_;

    public bool detectSwipeOnlyAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;

    void Awake() {
        // SystemInfo.deviceType = 
    }

    private List<MyTouch> getActiveTouches() {
        List<MyTouch> result = new List<MyTouch>();

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                result.Add(new MyTouch(touch.fingerId, touch.position));
        }

        return result;
    }

    void Update()
    {
        HashSet<int> used = new HashSet<int>();
        // List<bool> used = new List<bool>(startPositions.Count);

        foreach (MyTouch touch in getActiveTouches()) {
            used.Add(touch.id);
            
            if (!swipes_.ContainsKey(touch.id))
            {
                swipes_[touch.id] = new SwiperData(touch.position);
                Debug.Log("Started" + touch.position);
                continue;
            }

            // That means touch is already exists
            if (!detectSwipeOnlyAfterRelease)
            {
                Vector2 startPosition = swipes_[touch.id].startPosition;
                checkSwipe(startPosition, touch.position);

                swipes_[touch.id] = new SwiperData(startPosition, touch.position);
            }
            Debug.Log("moved" + touch.position);
        }

        List<int> keys = this.swipes_.Keys.ToList();

        foreach (int key in keys) {
            if (!used.Contains(key)) {
                SwiperData swipe = swipes_[key]; 
                
                checkSwipe(swipe.startPosition, swipe.lastPosition);
                
                this.swipes_.Remove(key);
            }
        }

        

        // foreach (Touch touch in Input.touches)
        // {
        //     // touch.fingerId
        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         fingerUp = touch.position;
        //         fingerDown = touch.position;
        //         Debug.Log("Started" + touch.position);
        //     }

        //     //Detects Swipe while finger is still moving
        //     if (touch.phase == TouchPhase.Moved)
        //     {
        //         if (!detectSwipeOnlyAfterRelease)
        //         {
        //             fingerDown = touch.position;
        //             checkSwipe();
        //         }
        //         Debug.Log("moved" + touch.position);
        //     }

        //     //Detects swipe after finger is released
        //     if (touch.phase == TouchPhase.Ended)
        //     {
        //         fingerDown = touch.position;
        //         checkSwipe();
        //         Debug.Log("ended" + touch.position);
        //     }
        // }
    }

    void checkSwipe(Vector2 start, Vector2 end)
    {
        float magnitude = (end - start).magnitude;

        if (magnitude > SWIPE_THRESHOLD) {
            Debug.Log("Swipe");
        }
    }
}
