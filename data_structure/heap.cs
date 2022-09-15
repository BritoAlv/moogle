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
        foreach (var item in result)
        {
            Console.WriteLine(item);
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
        /*
        question: in what part of the code is used that the list are sorted ?
        */
        List<id_element<T>> result = new List<id_element<T>>();
        min_heap<id_element<T>> b = new min_heap<id_element<T>>();
        int total = 0;
        foreach (var item in lists) // loop through each of the diferent id lists
        {            
            int templ = item.Value.Count; // number of elements with that same id,
            if (templ > 0) 
            {
                b.insert(item.Value[0]); // insert first value of each list in the heap
                total = total + templ; // sum the number of element with this id
            }
        }
        // now total stores all elements that there were in the "lists", 
        for (int i = 0; i < total; i++) // now we start the process.
        {
            id_element<T> temp = b.extract_min(); // extract min element from the heap
            lists[temp.id].RemoveAt(0); // remove this element from its respective list in the dict of ids
            result.Add(temp); // add this element to the final result
            if (lists[temp.id].Count > 0) // if this list have at least one element more, we put its first element into the heap.
            {
                b.insert(lists[temp.id][0]); // put in the heap first elemnt in the list.
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
        // compute index of childs of index.
        int chil1 = 2*index+1; 
        int chil2 = 2*index+2;

        // store A[index] in a  temp variable
        T temp = this.A[index];
        if ( chil2 < this.A.Count ) // index have two children
        {
            // set variable child to menor of the two indexes.
            int child = chil2;
            if (this.A[chil1].CompareTo(this.A[chil2]) < 0)
            {
                child = chil1;
            }
            if (this.A[child].CompareTo(this.A[index]) < 0)
            {
                this.A[index] = this.A[child];
                this.A[child] = temp;
                fix_child(child);
            }

        }
        else if (chil2 == this.A.Count) // have only left children
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