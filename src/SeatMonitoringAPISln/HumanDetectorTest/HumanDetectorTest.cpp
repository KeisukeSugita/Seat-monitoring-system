#define BOOST_TEST_MAIN
#include <boost/test/included/unit_test.hpp>
#include "opencv2/opencv.hpp"
#include <vector>
#include <regex>
#include <fstream>
#include <string>
#include "../HumanDetector/HumanDetector.h"


using namespace cv;
using namespace std;

BOOST_AUTO_TEST_CASE(HumanDetectorTest)
{
	Mat mat = imread("C:\\Users\\z00s600157\\Pictures\\SeatMonitoringAPITest—p‰æ‘œ\\Exists‰æ‘œ.jpg", 1);
	
	bool result = detect(mat.rows, mat.cols, mat.ptr());

	BOOST_CHECK_EQUAL(result, true);
}
