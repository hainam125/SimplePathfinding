using System;

public class Heap<T> where T : Node {
    private int[] itemHeapIdx;
	private T[] items;
	private int currentItemCount;
    private Func<T, T, int> Compare;

    public int Count { get { return currentItemCount; } }

    public Heap(int maxHeapSize, Func<T, T, int> compare) {
		items = new T[maxHeapSize];
        itemHeapIdx = new int[maxHeapSize];
        Compare = compare;
	}
	
	public void Add(T item) {
        itemHeapIdx[item.Id] = currentItemCount;
		items[currentItemCount] = item;
		SortUp(item);
		currentItemCount++;
	}

	public T RemoveFirst() {
		T firstItem = items[0];
		currentItemCount--;
		items[0] = items[currentItemCount];
        itemHeapIdx[items[0].Id] = 0;
		SortDown(items[0]);
		return firstItem;
	}

	public void UpdateItem(T item) {
		SortUp(item);
	}

	public bool Contains(T item) {
        return Equals(items[itemHeapIdx[item.Id]], item);
	}

	void SortDown(T item) {
		while (true) {
            int childIndexLeft = itemHeapIdx[item.Id] * 2 + 1;
            int childIndexRight = itemHeapIdx[item.Id] * 2 + 2;
            int swapIndex = 0;

			if (childIndexLeft < currentItemCount) {
				swapIndex = childIndexLeft;

                if (childIndexRight < currentItemCount && Compare(items[childIndexLeft], items[childIndexRight]) < 0)
                {
                    swapIndex = childIndexRight;
                }

                if (Compare(item, items[swapIndex]) < 0) Swap (item,items[swapIndex]);
				else return;
			}
			else {
				return;
			}

		}
	}
	
	void SortUp(T item) {
        int parentIndex = (itemHeapIdx[item.Id] - 1) / 2;

        while (true) {
			T parentItem = items[parentIndex];

            if (Compare(item, parentItem) > 0) Swap (item,parentItem);
			else break;
            
            parentIndex = (itemHeapIdx[item.Id] - 1) / 2;
        }
	}
	
	void Swap(T itemA, T itemB) {
        items[itemHeapIdx[itemA.Id]] = itemB;
        items[itemHeapIdx[itemB.Id]] = itemA;
        int itemAIndex = itemHeapIdx[itemA.Id];
        itemHeapIdx[itemA.Id] = itemHeapIdx[itemB.Id];
        itemHeapIdx[itemB.Id] = itemAIndex;
    }
}
