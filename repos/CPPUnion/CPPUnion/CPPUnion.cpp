#include <stdio.h>

union bir {
	int id;
	double dd;
	char cd;
} bd;

int main(void)
{
	bd.id = 21;
	bd.cd = 'A';

	printf("%d %c\n", bd.id, bd.cd); /* 1 */

	bd.id = 127;
	printf("%d %c\n", bd.id, bd.cd); /* 2 */

	bd.dd = 34.75;
	printf("%d %f", bd.id, bd.dd);   /* 3 */

	return 0;
}
