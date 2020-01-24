#include <stdio.h>
#include <stdlib.h>
float a;
float b;
void main(int argc, char* argv[]){
	//  ***** Local declarations *****
	//  ***** Code Body *****
	printf("res=%f\n",a=0);
	while ( a<10 ){
		//  ***** Local declarations *****
		//  ***** Code Body *****
		printf("res=%f\n",a=a+1);
		printf("res=%f\n",b=a+10);
		if ( b>15 ){
			//  ***** Local declarations *****
			//  ***** Code Body *****
			break;
			
		}
		
	}
	
}
float foo(float a, float b){
	//  ***** Local declarations *****
	float c;
	//  ***** Code Body *****
	if ( a>1 ){
		//  ***** Local declarations *****
		//  ***** Code Body *****
		printf("res=%f\n",c=b);
		
	}
	printf("res=%f\n",c=a+b);
	return c;
	
}

