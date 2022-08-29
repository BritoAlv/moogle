public class trie {
    private TrieNode root;

    public trie() {
        root = new TrieNode();
    }
    public void Insert(string word) {
        var node = root;
        foreach (var ch in word) {
            if (!node.ContainsKey(ch)) {
                node.Add(ch, new TrieNode());
            }
            node = node.Get(ch);
        }
        node.SetComplete();
    }
    // Search a prefix or whole key in trie and
    // returns the node where search ends
    public TrieNode SearchPrefix(string word) {
        var node = root;
        foreach (var ch in word) {
            if (node.ContainsKey(ch)) {
                node = node.Get(ch);
            } else {
                return null;
            }
        }
        return node;
    }
    public bool Search(string word) {
        var node = SearchPrefix(word);
        return node != null && node.IsComplete();
    }
    public bool StartsWith(string prefix) {
        var node = SearchPrefix(prefix);
        return node != null;
    }
}

// TrieNode
public class TrieNode {
    private Dictionary<char, TrieNode> children;
    private bool isComplete;
    private int count;

    public TrieNode() {
        children = new Dictionary<char, TrieNode>();
    }
    public bool ContainsKey(char ch) {
        return children.ContainsKey(ch);
    }
    public TrieNode Get(char ch) {
        return children[ch];
    }
    public void Add(char ch, TrieNode node) {
        children[ch] = node;
    }
    public void SetComplete() {
        isComplete = true;
        ++count;
    }
    public bool IsComplete() {
        return isComplete;
    }
    public int WordCount() {
        return count;
    }
}