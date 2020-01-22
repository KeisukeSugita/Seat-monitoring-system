#include "HumanDetector.h"
#include "opencv2/opencv.hpp"

using namespace cv;
using namespace std;

bool detect(int rows, int cols, unsigned char* image)
{
	Mat mat(rows, cols, CV_8UC3, image, Mat::AUTO_STEP);	// �摜�f�[�^��Mat�^�ɕϊ�
	
	CascadeClassifier cascade;	//	�J�X�P�[�h���ފ�i�[�ꏊ
	cascade.load("SeatMonitoringAPI\\cascades\\haarcascade_frontalface_alt.xml");	//	���ʊ��񂪓����Ă���J�X�P�[�h
	if (cascade.empty())
	{
		throw runtime_error("�J�X�P�[�h���ފ킪�ǂݍ��߂܂���ł����B");
	}

	vector<Rect> faces;	//	�֊s���̊i�[�ꏊ

	cascade.detectMultiScale(mat, faces, 1.1, 3, 0, Size(20, 20));	//	�i�[���ꂽ�t���[���ɑ΂��ăJ�X�P�[�h�t�@�C���Ɋ�Â��Ċ�����m
	
	return 0 < faces.size();
}