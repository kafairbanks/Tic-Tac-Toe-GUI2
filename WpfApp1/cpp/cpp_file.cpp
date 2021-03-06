// cpp.cpp : Defines the entry point for the console application.
//Wrapper for ComputerWrapper
#pragma unmanaged
#include "cpp_file.h"
#include <fstream>


cpp_file::cpp_file( void ) {
	//main instance of overall ai
	c = new ComputerWrapper();
}

cpp_file::~cpp_file( void ) {
	//allows for the ComputerWrapper to clear the board state and delete the Computer instance that is inside the ComputerWrapper
	c->resetComputer();
	delete c;
}

int cpp_file::aiCall( int playerMove, int* compMoves ) {

	//computerCall overwrites the compMoves with the computer's move, 4 winning moves (if applicable) and a 1 if the game is won
	c->computerCall( compMoves, playerMove );

	return 0;
}