public class query
{
    // steps to build a query object, query is a string,
    // first separate words in query in a list of arrays of string, each array represent a term, array with more of two words mean that they are atachhed by ~. at the same time we do this we store in a dict the tf-idf of each word in the query, (each word, not term).
   // so after we make a function to compute similarity between a document and the query, and now we implement the operators like if term has an ! we don't include this documents, if it has * we multiply itf-idf of document, first sort document by cosine-similarity and after that we sort this the document by closest_words algorithm.
   // steps to build query, and such, there is to implement a stemmer for the word in the query, we create a dict with all words in the query and compute its tf-idf, like usual, now we add to the document the term that are related by the closest_operator we obtain its positions, and its tf-idf is measured by how         
   
   
}
