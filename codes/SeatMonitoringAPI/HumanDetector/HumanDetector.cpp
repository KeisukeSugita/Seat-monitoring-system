#include "HumanDetector.h"
#include "opencv2/opencv.hpp"

using namespace cv;
using namespace std;

bool detect(int rows, int cols, unsigned char* image)
{
	Mat mat(rows, cols, CV_8UC3, image, Mat::AUTO_STEP);	// 画像データをMat型に変換
	
	CascadeClassifier cascade;	//	カスケード分類器格納場所
	cascade.load("SeatMonitoringAPI\\cascades\\haarcascade_frontalface_alt.xml");	//	正面顔情報が入っているカスケード
	if (cascade.empty())
	{
		throw runtime_error("カスケード分類器が読み込めませんでした。");
	}

	vector<Rect> faces;	//	輪郭情報の格納場所

	cascade.detectMultiScale(mat, faces, 1.1, 3, 0, Size(20, 20));	//	格納されたフレームに対してカスケードファイルに基づいて顔を検知
	
	return 0 < faces.size();
}