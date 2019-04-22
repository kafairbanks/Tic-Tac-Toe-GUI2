#pragma unmanaged
#include "ComputerWrapper.h"

#ifdef CPP_EXPORTS
#define CPP_API __declspec(dllexport)
#else
#define CPP_API __declspec(dllimport)
#endif

class CPP_API cpp_file
{
private:
	ComputerWrapper * c;

public:
	cpp_file( void );
	~cpp_file( void );

	int aiCall( int playerMove, int* compMoves );
};