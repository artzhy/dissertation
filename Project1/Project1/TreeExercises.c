* Time-stamp: <Mon Jan 17 2011 14:38:55 Stardate: Stardate: [-28]4307.84 hwloidl>

   Course: Parallel and Distributed Technology
   Class:  C Revision (Part 1)
   Example: tree structures in C

   Compile: gcc -g -o crev2 crev2.c
*/

* includes */
#include <stdio.h>
#include <stdlib.h>

* max number of arguments to read from the command line */
#define MAX 99

* externs */

* types */
* enumeration of possible tags of type int  */
* Exercise: use a less wasteful representation */
typedef enum { Leaf = 0, Branch = 1 } Tag;
* define composition of a branch */
struct branch {struct tree *left, *right;};
* define alternatives of a node */
union node {struct branch b; int value;};
* define a tree as a tagged node */
struct tree { Tag tag; union node n; };

* prototypes */
void         showArr (int len, int *arr);
struct tree *mkTree(int from, int to, int *arr);
struct tree *readTree(FILE *fin, int n);
void         showTree(int n, struct tree *t);
int treeIsBalanced(struct tree * t);
int max(int a, int b);
int noNodesOnBranch(struct tree * t);

* code */
int main (int argc, char **argv) {
  struct tree *t, *ta, *tb;

  int a[4] = { 1, 2, 3, 4 };
  t = (struct tree *) malloc(sizeof(struct tree));

  
  printf("Tree of consisting of these values:\n");
  showArr(4,a);
  ta = mkTree(0,3,a);
  showTree(0,ta);

  /* check command line */
  if (argc<2) {
    printf("Usage: %s <n1> ...\n", argv[0]);
    printf("       to build a tree, containing integers <n1> ...\n");
    exit(1);
  }

  if (argc>MAX) {
    printf("Can only take up to %d arguments\n", MAX);
    exit(1);
  }

  { /* nested scope */

    /* build and show a 4-element tree */
    int b[MAX];
    int i; /* local */
    printf("Constructing tree out of command line arguments ...\n");
    for (i=1; i<argc; i++) {  /* read tree elements from command line */
      b[i-1] = atoi(argv[i]); /* beware of indexing */
    }
    tb = mkTree(0, argc-2, b);/* build the tree */
    showTree(0, tb);


	printf("%d", treeIsBalanced(tb));
  }
  /* beware, b doesn't exist here any more */    
}

int treeIsBalanced(struct tree * t) {
	if(t == NULL)
		return 1;
	else {
		// Count nodes on left branch
		int heightLeft = noNodesOnBranch(t->n.b.left);
		// Count nodes on right branch
		int heightRight = noNodesOnBranch(t->n.b.right);
		
		if(abs(heightLeft - heightRight) <= 1 && treeIsBalanced(t->n.b.left) && treeIsBalanced(t->n.b.right))
			return 1;
 
   /* If we reach here then tree is not height-balanced */
   return 0;

		return 0;
	}
}

int max(int a, int b) {
	if(a == b) 
		return a;
	else if(a > b) 
		return a;
	else 
		return b;
}


int noNodesOnBranch(struct tree * t) {
	if(t == NULL)
		return 0;
	else 
		return 1 + max(noNodesOnBranch(t->n.b.left), noNodesOnBranch(t->n.b.right));
}

* build a balanced tree from an array segment */
struct tree * mkTree(int from, int to, int *arr) {
  if (from>to) {
    return (struct tree *)NULL;
  } else if (from==to) {
    struct tree *t;
    t = (struct tree *) malloc(sizeof(struct tree));    
    t->tag = Leaf; 
    t->n.value = arr[from];
    return t;
  } else {
    struct tree *t, *left, *right;
    int mid = (from + to) / 2;
    t = (struct tree *) malloc(sizeof(struct tree));    
    left  = mkTree(from,mid,arr);
    right = mkTree(mid+1,to,arr);
    t->tag = Branch;
    t->n.b.left = left;
    t->n.b.right = right;
    return t;
  }
}

void showTree(int n, struct tree *t) {
  int i;
  for (i=0; i<n; i++)
    putc( ' ', stdout);
  switch (t->tag) {
  case Leaf: 
    printf("%d", t->n.value);
    break;
  case Branch:
    putc( '.', stdout);
    putc( '\n', stdout);
    showTree(n+1, t->n.b.left);
    showTree(n+1, t->n.b.right);
    break;
  }
  putc( '\n', stdout);
}

void showArr (int len, int *arr) {
  int i;
  printf("Array at %x: \n", arr);
  for (i=0; i<len; i++) {
    printf("%d: %d\n", i, arr[i]);
  }
}
