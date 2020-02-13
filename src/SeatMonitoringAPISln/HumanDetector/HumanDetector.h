#pragma once

using namespace std;

extern "C"
{
	__declspec(dllexport)  bool __stdcall detect(int rows, int cols, unsigned char* image,  const char* cascadeFile);
}
