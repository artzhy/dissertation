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

/* code */
int main () {

  /* data layout */
  printf("\nTesting data layout issues\n");
  unsigned long int l;
  /* l = 0x01234567; */
  l = 0x0000aabbccddeeff;

  printf("l: %lu\n", l);

  /* arrays stand for addresses */
  printf("\nTesting arrays\n");
  int a[3] = { 1, 2, 3 };
  /* unsigned long int a[3] = { 1, 2, 3 }; */  /* this version uses 2 words per int */
  printf("a: 0x%x; &a: 0x%x\n", a, &a);

  /* array access through pointer arithmetic */
  printf("2nd element of a: %d; %d\n", a[1], *(a+1));
  /* beware of ill-typed memory access like this: *(unsigned long *)(a+1) */

  /* this uses byte-based pointer arithmetic; notice the type cast to (short *) to assure
     that the addition is done in terms of bytes */
  printf("3rd element of a (element size: %d): %d; %d\n", sizeof(int), a[2], *((char *)a+2*sizeof(int)));

  /* using a function */
  showArr(3,a);

  /* show sizes */
  showSizes();

  /* structures */
  printf("\nTesting structures\n");
  struct { 
    int  i;
    char c;
    int  a[3]; }  s;

  s.i = 1;
  s.c='a';
  s.a[0] = 5; 
  s.a[1] = 6; 
  s.a[2] = 7;

  printf("structure\ns: %d %c\n", s.i, s.c);
  showArr(3,s.a);

  /* lists */
  printf("\nLists, implemented using structures\n");
  node *x;
  x = mkList(3, a);
  showList(x);

  /* type coercions */
  printf("\nTesting coercions\n");
  unsigned long int l1;  
  char * c;
  l1 = 0xaabbccdd;
  printf("l1 (a long int, stored in little-endian format): %x\n", l1);
  c = (char *)(&l1);
  printf("as array of chars (note the inverse order): 0x%x 0x%x 0x%x 0x%x\n",
	 c[0],c[1],c[2],c[3]);

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

/* generate a list from an array */
/* Exercise: modify the code to eliminate the code 
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