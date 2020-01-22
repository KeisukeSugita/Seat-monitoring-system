#pragma once

extern "C"
{
	__declspec(dllexport) bool detect(int rows, int cols, unsigned char* image);
}
