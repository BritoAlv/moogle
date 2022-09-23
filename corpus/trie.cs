namespace corpuss;
public class trie_node{
    /*
    this represents the node of a trie. contains the char that it holds, a dict
    to point to all of its children, if it is a word, knows its parent. 
    */
    public char c;
    public trie_node parent;
    public Dictionary<char, trie_node> children = new Dictionary<char, trie_node>();
    public bool is_word;
    public trie_node(){}
    public trie_node(char c){
        this.c = c;
    }
}

public class trie{
    private trie_node root; // root of the trie, in esence this is all the info of a trie.
    /*
    Containers for global variables to store information (this is for store the words found in the search of
    words that start with especific substring) 
    */
    List<string > words; // a container to save words that start with prefix root, like a global variable for the trie
    trie_node prefix_root; // the root of the prefix
    string current_prefix; // the prefix

    public trie(){
        root = new trie_node();
        words = new List<string>();
    }

    public void insert(string word)
    {
        // we start initially at the root of the trie, 
        Dictionary<char, trie_node> children = this.root.children;
        trie_node current_parent = root;
        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];
            trie_node t; // node of the trie that we should be
            if (children.ContainsKey(c))
            {
                t = children[c];
            }
            else
            {
                t = new trie_node(c);
                t.parent = current_parent;
                children[c] = t;

            }
            // move to the next node
            children = t.children;
            current_parent = t;

            // if end set is_word to true
            if(i == word.Length-1)
            {
                t.is_word = true;
            }
        }
    }

    public bool search(string word)
    {
        trie_node t = search_node(word);
        if(t != null && t.is_word)
        {
            return true;
        }
        return false;
    }

    public bool startsWith(string prefix)
    {
        if(search_node(prefix) == null) // because if not, this mean that at least a word with that prefix was added.
        {
            return false;
        }
        return true;
    }

    public trie_node search_node(string prefix)
    {
        // search the node at that prefix, and set as node in the global info of the trie as prefix root.
        Dictionary<char, trie_node> children = root.children;
        // may be null if not is found 
        trie_node t = null;
        // notice that if for some i the key is not found this directly return null.
        for (int i = 0; i < prefix.Length; i++)
        {
            char c = prefix[i];
            if (children.ContainsKey(c))
            {
                t = children[c];
                children = t.children;
            }
            else
            {
                return null;
            }
        }
        // update gloabl info
        prefix_root = t;
        current_prefix = prefix;
        words.Clear();

        // return the trieNode
        return t;
    }

    public void words_finder_traversal(trie_node node)
    {
        if(node.is_word == true)
        {
            trie_node altair; // find the father of node which will be altair.
            altair = node; // by default it is itself
            Stack<string> hstack = new Stack<string>();
            while (altair != prefix_root) // prefix root was updated with search node before
            {
                hstack.Push( altair.c.ToString());
                altair = altair.parent;
            }
            string wrd = current_prefix;
            while (hstack.Count() >0)
            {
                wrd = wrd + hstack.Pop();
            }

            words.Add(wrd);
        }
        // loop through all children ob this node. this may be a lot of time consumer, especially if there
        // are 180K words.
        HashSet<char> kset = node.children.Keys.ToHashSet();
        foreach (var item in kset)
        {
            words_finder_traversal(node.children[item]);
        }
    }
    public string search_word(string prefix, corpus x)
    {
        if( this.startsWith(prefix))
        {
            trie_node tn = this.search_node(prefix); // search for the node that starts at "prefix"
            this.words_finder_traversal(tn); // find all words at that root, and save them in the global list
            string result = this.words[0];
            int max = x.bd[this.words[0]].idf;
            foreach (var word in this.words)
            {
                if(max < x.bd[word].idf)
                {
                    max = x.bd[word].idf;
                    result = word;
                }
            }
            return result; // get the word with bigger idf from all the words.
        }
        else
        {
            return prefix;
        }
        
    }
}