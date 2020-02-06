#include "HumanDetector.h"
#include <vector>
#include "opencv2/opencv.hpp"
#include <windows.h>
#include <tchar.h>
#define GetHInstance() ((HINSTANCE)GetModuleHandle("HumanDetector.dll"))

using namespace cv;
using namespace std;


extern "C" bool detect(int rows, int cols, unsigned char* image, const char* cascadeFile)
{
	char path[MAX_PATH + 1];

	GetModuleFileName(GetHInstance(), path, MAX_PATH);


	Mat mat(rows, cols, CV_8UC3, image, Mat::AUTO_STEP);	// 画像データをMat型に変換
	
	CascadeClassifier cascade;	//	カスケード分類器格納場所
	cascade.load(cascadeFile);	//	正面顔情報が入っているカスケード

	vector<Rect> faces;	//	輪郭情報の格納場所

	cascade.detectMultiScale(mat, faces, 1.1, 3, 0, Size(20, 20));	//	格納されたフレームに対してカスケードファイルに基づいて顔を検知
	
	return 0 < faces.size();
}