/* Time-stamp: <Mon Jan 17 2011 14:43:05 Stardate: Stardate: [-28]4307.85 hwloidl>

   Course: Parallel and Distributed Technology
   Class:  C Revision (Part 1)
   Example: data layout: little-endian 

   Compile: gcc -g -o crev1 crev1.c
   Run (in debugger):  gdb t1
                       # r

   Demo:
   . set break-point at printf statement
   . print value of array a, address of array a and contents of address of a
*/

/* includes */
#include <stdio.h>
#include <stdlib.h>

/* externs */

/* types */
typedef struct _node { int value; struct _node *next; } node;
typedef node *list;
// the above makes list equivalent to (struct node *)

/* prototypes */
void showArr (int len, int *arr);
void showSizes ();
node *mkList(int len, int *arr);
void showList(node *x);
node * append(node *x, node *y);
void DemoListAppendModification();
void DemoListAppendNoModification();
node * nonModAppend(node * x, node * y);
node * reverse(node * x);
void DemoReverse();

/* code */
int main () {
  DemoListAppendModification();
  DemoListAppendNoModification();
  DemoReverse();
  scanf("%c");
}

void DemoReverse() {
  node *x, *y;
  int a[3] = {1, 2, 3};
    
  printf("\n\n\n--Demonstrate List Reverse function, with modification of input list --\n");

  x = mkList(3, a);
  showList(x);
  showList(reverse(x));
}

void DemoListAppendModification() {
  node *x, *y;
  int a[2] = {1, 2};
  int b[2] = {3, 4};
  
  printf("\n\n\n--Demonstrate List Append function, with modification of input list --\n");

  x = mkList(2, a);
  y = mkList(2, b);
  
  showList(x);
  showList(y);

  showList(append(x, y));
}

void DemoListAppendNoModification() {
  node *x, *y;
  int a[2] = {1, 2};
  int b[2] = {3, 4};
  
  printf("\n\n\n--Demonstrate List Append function, with modification of input list --\n");

  x = mkList(2, a);
  y = mkList(2, b);
  
  printf("--Input Lists --\n");
  showList(x);
  showList(y);

  printf("--Output List --\n");
  showList(nonModAppend(x, y));

  printf("--Input Lists --\n");
  showList(x);
  showList(y);
}

node * reverse(node * x) {
  node * newRoot = 0;
  while (x) {
    node * next = x->next;
    x->next = newRoot;
    newRoot = x;
    x = next;
  }
  return newRoot;

}

node * nonModAppend(node * x, node * y) {
	// Create a new list
	node * root = NULL;
	node * last, * current, * z, * a;
	z = x;
	a = y;

	// Loop through X and add to newList
	while(z != NULL) {
		if(root == NULL) {
			current = (node *) malloc(sizeof(node));
			current->value = z->value;
			root = current;
			
		} else {
			current = (node *) malloc(sizeof(node));
			current->value = z->value;
			last->next = current;
		}
		last = current;
		z = z->next;
	}

	// Loop through Y and add to newList
	while(a != NULL) {
		if(root == NULL) {
			current = (node *) malloc(sizeof(node));
			current->value = a->value;
			root = current;
		} else {
			current = (node *) malloc(sizeof(node));
			current->value = a->value;
			last->next = current;
		}
		last = current;
		a = a->next;
	}

	last->next = NULL;
	return root;	
}

node * append(node * x, node * y) {
	// Loop through x, and add y to the last element of x.
	
	node * last, *z;
	z = x;
	while(z->next != NULL) {
		last = z->next;
		z = x->next;
	}
		
	last->next = y;

	return x;	
}

void showArr (int len, int *arr) {
  int i;
  printf("Array at %x: \n", arr);
  for (i=0; i<len; i++) {
    printf("%d: %d\n", i, arr[i]);
  }
}

void showSizes () {
  printf("Printing sizes of basic C data types:\n");
  printf("size of int: %d\n", sizeof(int));
  printf("size of short: %d\n", sizeof(short));
  printf("size of long: %d\n", sizeof(long));
  printf("size of char: %d\n", sizeof(char));
  printf("size of float: %d\n", sizeof(float));
  printf("size of double: %d\n", sizeof(double));
  printf("size of int*: %d\n", sizeof(int*));
}

* generate a list from an array */
* Exercise: modify the code to eliminate the code 
             duplication in if and while */
node *mkList(int len, int *arr) {
  int i;
  node *curr, *last, *root;
  if (len>0) {
    last = (node*) malloc(1*sizeof(node));
    last->value = arr[0];
    root = last;
  } else {
    return NULL;
  }
 
  for (i=0; i<len-1; i++) {
    curr = (node*) malloc(1*sizeof(node));
    curr->value = arr[i+1];
    last->next = curr;
    last = curr;

  }
  last->next = NULL;
  return root;
}

void showList(node *x) {
  printf("List at %x: ", x);
  while (x!=NULL) { 
    printf(", %d", x->value);
    x = x->next;
  }
  printf("\n");
}
