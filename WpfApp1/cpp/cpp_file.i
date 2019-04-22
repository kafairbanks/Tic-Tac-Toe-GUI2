%module cpp
 
%{
    #include "cpp_file.h"
%}
%include "arrays_csharp.i" 
%include <windows.i>

%apply int OUTPUT[] {int *compMoves}
%include "cpp_file.h"