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


	Mat mat(rows, cols, CV_8UC3, image, Mat::AUTO_STEP);	// �摜�f�[�^��Mat�^�ɕϊ�
	
	CascadeClassifier cascade;	//	�J�X�P�[�h���ފ�i�[�ꏊ
	cascade.load(cascadeFile);	//	���ʊ��񂪓����Ă���J�X�P�[�h

	vector<Rect> faces;	//	�֊s���̊i�[�ꏊ

	cascade.detectMultiScale(mat, faces, 1.1, 3, 0, Size(20, 20));	//	�i�[���ꂽ�t���[���ɑ΂��ăJ�X�P�[�h�t�@�C���Ɋ�Â��Ċ�����m
	
	return 0 < faces.size();
}