using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Meg.Maths;
using Meg.Networking;

public class DiagnosticDataFeed : MonoBehaviour
{

    // Properties
    // ------------------------------------------------------------

    [Header("Configuration")]

    /** Server data value. */
    public string LinkDataString;

    /** Data value range. */
    public Vector2 DataRange = new Vector2(0, 100);

    /** Probability for error items based on data value. */
    public AnimationCurve ErrorRateCurve;

    /** Probability for warning items based on data value. */
    public AnimationCurve WarningRateCurve;

    /** Probability for pending items based on data value. */
    public AnimationCurve PendingRateCurve;

    /** Probability for normal items based on data value. */
    public AnimationCurve NormalRateCurve;

    /** Numer of items to display in feed. */
    public int ItemCount = 20;

    /** Position offset between each item. */
    public Vector3 OffsetBetweenItems = new Vector3(0, -0.18f, 0);

    /** Update interval between successive feed items. */
    public float UpdateInterval = 1.0f;

    /** Shuffle interval for feed items. */
    public float MoveDuration = 0.25f;


    [Header("Prefabs")]

    /** Prefab to use for line items. */
    public DiagnosticFeedItem ItemPrefab;


    // Private Properties
    // ------------------------------------------------------------


    // Members
    // ------------------------------------------------------------

    private readonly Queue<DiagnosticFeedItem> _items = new Queue<DiagnosticFeedItem>();



    // Unity Methods
    // ------------------------------------------------------------

    private void OnEnable()
    {
	    StopAllCoroutines();
	    StartCoroutine(FeedRoutine());
    }

    private void OnDisable()
    {
    }

    private IEnumerator FeedRoutine()
    {
        // Clear any old items out.
        while (_items.Count > 0)
            Destroy(_items.Dequeue().gameObject);

        // Add in initial items.
        var position = Vector3.zero;
        while (_items.Count < ItemCount)
        {
            _items.Enqueue(AddFeedItem(position, false));
            position += OffsetBetweenItems;
        }

        // Animate items over time.
        var wait = new WaitForSeconds(UpdateInterval);
        var last = OffsetBetweenItems * ItemCount;
        while (gameObject.activeSelf)
        {
            // Wait a bit.
            yield return wait;

            // Add a new item.
            _items.Enqueue(AddFeedItem(last, true));

            // Shuffle items up.
            foreach (var item in _items)
                item.transform.DOLocalMove(-OffsetBetweenItems, MoveDuration).SetRelative(true);
        }
    }

    private DiagnosticFeedItem AddFeedItem(Vector3 position, bool animate)
    {
        while (_items.Count > ItemCount)
        {
            var old = _items.Dequeue();
            Destroy(old.gameObject);
        }

        var item = Instantiate(ItemPrefab);
        item.transform.SetParent(transform, false);
        item.transform.localPosition = position;
        item.Configure(GetItemType(), animate);
        return item;
    }

    private DiagnosticFeedItem.ItemType GetItemType()
    {
        var dataValue = serverUtils.GetServerData(LinkDataString);
        var t = graphicsMaths.remapValue(dataValue, DataRange.x, DataRange.y, 0, 1);

        if (Random.value < ErrorRateCurve.Evaluate(t))
            return DiagnosticFeedItem.ItemType.Error;
        if (Random.value < WarningRateCurve.Evaluate(t))
            return DiagnosticFeedItem.ItemType.Warning;
        if (Random.value < PendingRateCurve.Evaluate(t))
            return DiagnosticFeedItem.ItemType.Pending;
        if (Random.value < NormalRateCurve.Evaluate(t))
            return DiagnosticFeedItem.ItemType.Normal;

        return DiagnosticFeedItem.ItemType.Inactive;
    }
}
