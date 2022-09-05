/*
1
22
33
122333
012345
*/

namespace d_t_h;

public static class idk{
    /*
    merge list of same type of objects
    */
    public static List<T> merge_k_list<T>(params List<T>[] lists) where T:IComparable
    {
        Dictionary<int, List<id_element<T>>> a = new Dictionary<int, List<id_element<T>>>();
        for (int i = 0; i < lists.Count(); i++)
        {
            a[i] = new List<id_element<T>>();
            foreach (var item in lists[i])
            {
                id_element<T> temp = new id_element<T>(i, item);
                a[i].Add(temp);
            }
        }
        List<id_element<T>> c = merge_k_list(a);
        List<T> result = new List<T>();
        foreach (var item in c)
        {
            result.Add(item.val);
        }
        return result;
    }


    /* 
    merge list of diferent type of objects.
    */
    public static List<id_element<T>> merge_k_list<T>(Dictionary< int, List<id_element<T>>> lists) where T: IComparable
    {
        // takes k sorted lists of elements of id_element we use char to identifie the list,
        // so two elements in the same list have sAme char value.
        List<id_element<T>> result = new List<id_element<T>>();
        min_heap<id_element<T>> b = new min_heap<id_element<T>>();
        int total = 0;
        foreach (var item in lists)
        {
            int templ = item.Value.Count();
            if (templ > 0)
            {
                b.insert(item.Value[0]);
                total = total + templ;
            }
        }
        for (int i = 0; i < total; i++)
        {
            id_element<T> temp = b.extract_min();
            lists[temp.id].RemoveAt(0);
            result.Add(temp);
            if (lists[temp.id].Count > 0)
            {
                b.insert(lists[temp.id][0]);
            }            
        }
        return result;
    }
}

public class id_element<U>: IComparable<id_element<U>> where U:IComparable
{
    public int id;
    public U val;

    public id_element(int id, U val)
    {
        this.id = id;
        this.val = val;
    }

    public int CompareTo(id_element<U> other)
    {
        return this.val.CompareTo(other.val);
    }

    public bool equal(id_element<U> other)
    {
        return this.id == other.id;
    }

    // given a list of objects of type id_element<U> return the number of diferent ids in it.
    public int number_diferent(List<id_element<U>> A)
    {
        List<int> ids = new List<int>();
        foreach (var item in A)
        {
            if (!ids.Contains(item.id))
            {
                ids.Add(item.id);
            }            
        }
        return ids.Count;
    }

    


}

public class min_heap<T> where T:IComparable<T>
{
    public List<T> A;
    public min_heap()
    {
        this.A = new List<T>();
    }
    public min_heap(IEnumerable<T> B)
    {
        this.A = new List<T>();
        foreach (var item in B)
        {
            this.insert(item);
        }
    }

    public T get_min()
    {
        return this.A[0];
    }

    public void fix_father(int index)
    {
        int father = (index-1)/2;
        if (father >=0)
        {
            if ( this.A[index].CompareTo(this.A[father]) < 0 )
            {
                T temp = this.A[father];
                this.A[father] = this.A[index];
                this.A[index] = temp;
                fix_father(father);
            }
        }
    }

    public void fix_child(int index)
    {
        int chil1 = 2*index+1;
        int chil2 = 2*index+2;
        T temp = this.A[index];
        if ( chil2 < this.A.Count )
        {
            
            int child = chil2;
            if (this.A[chil1].CompareTo(this.A[chil2]) < 0)
            {
                child = chil1;
            }
            this.A[index] = this.A[child];
            this.A[child] = temp;
            fix_child(child);
        }
        else if (chil2 == this.A.Count)
        {
            if (this.A[chil1].CompareTo(this.A[index]) < 0)
            {

                this.A[index] = this.A[chil1];
                this.A[chil1] = temp;
            }
        }
        else
        {
            // doesnt have either left or right childs so we are done.
        }    
    }

    public void insert(T val)
    {
        // insert a value into the heap
        int its = this.A.Count;
        this.A.Add(val);
        fix_father(its);
    }

    public T extract_min()
    {
        T temp = this.A[0];
        if (this.A.Count == 1)
        {
            this.A.RemoveAt(0);
        }
        else
        {
            this.A[0] = this.A.Last();
            this.A.RemoveAt(this.A.Count-1);
            fix_child(0);
        }
        return temp;
    }
}