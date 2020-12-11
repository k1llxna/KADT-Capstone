#ifndef DLIST_H
#define DLIST_H

#include <cstddef> // NULL

template <typename T>
class DList
{
	// DList node
	struct Node
	{
		T value;		// the value
		Node* next;		// next node
		Node* prev;		// previous node

		// a convenient constructor
		Node(const T& v) : value(v), next(NULL), prev(NULL) { }
	};

	// pointers to first and last nodes in the list
	Node* head, * tail;
	size_t sz;

public:

	// default constructor
	DList();

	// destructor
	~DList();

	// remove all nodes
	void clear();

	// check for empty list
	bool empty() const;

	size_t size() const;

	// return reference to first value in list
	// precondition: list is not empty
	T& front();

	// return reference to last value in list
	// precondition: list is not empty
	T& back();

	// insert a value at the beginning of the list
	void push_front(const T& value);

	// insert a value at the end of the list
	void push_back(const T& value);

	// remove first element from the list
	// precondition: list is not empty
	void pop_front();

	// remove last element from the list
	// precondition: list is not empty
	void pop_back();


	// Iterator class -- represents a position in the list.
	// Used to gain access to individual elements, as well as
	// insert, find, and erase elements
	class Iterator
	{
	protected:
		// the iterator holds a pointer to the "current" list node
		Node* node;

		// DList class needs access to private members of Iterator
		friend class DList;

		// private parameterized constructor: used by DList::begin() method
		Iterator(Node* n);

	public:

		// default constructor: iterator not valid until initialized
		Iterator();

		// equality operator (==)
		// checks whether both iterators represent the same position
		bool operator== (const Iterator& rhs) const;

		// inequality operator (!=)
		// checks whether iterators represent different positions
		bool operator!= (const Iterator& rhs) const;

		// dereference operator (unary *)
		// returns a reference to the data value at the iterator position
		// precondition: iterator is valid
		T& operator* () const;

		// preincrement operator
		// advances to next node and returns itself
		// precondition: iterator is valid
		Iterator& operator++();

		// postincrement operator
		// advances to next node and returns old copy of itself
		// precondition: iterator is valid
		Iterator operator++(int);

		// predecrement operator
		// advances to prev node and returns itself
		// precondition: iterator is valid
		Iterator& operator--();

		// postdecrement operator
		// advances to prev node and returns old copy of itself
		// precondition: iterator is valid
		Iterator operator--(int);
	};


	// return iterator set to first element in list
	Iterator begin();

	// return iterator set to last element in list (reverse begin)
	Iterator rbegin();

	// return iterator past last element in list
	Iterator end();

	// return iterator past first element in list (reverse end)
	Iterator rend();


	// erase the list element at pos
	// precondition: pos is a valid list iterator
	// returns: an iterator at the element immediately _after_ pos
	Iterator erase(Iterator pos);

	// insert a value before pos
	// precondition: pos is a valid iterator or end()
	// returns: an iterator at the inserted value
	Iterator insert(Iterator pos, const T& value);

	// find a value in the list
	// returns: a valid iterator if found, end() if not found
	Iterator find(const T& value);
};

#endif