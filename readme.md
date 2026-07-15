# Home Page
## Cv2.Threshold(grayMat, dstMat, thresholdValue, maxValue, type);
#### grayMat: 처리할 Gray Scale 이미지
#### dstMat: 처리된 scale 이미지
#### thresholdValue: 임계값
#### maxValue: 임계값을 넘었을 때 적용할 값
#### type: 임계점을 처리하는 방식

## Threshold Type → Binary, BinaryInv, Trunc, Tozero, TozeroInv, Ostu
#### ThresholdTypes.Binary: 임계값보다 크면 maxValue, 작으면 0
#### ThresholdTypes.BinaryInv: 임계값보다 크면 0, 작으면 maxValue
#### ThresholdTypes.Trunc: 임계값보다 크면 임계값, 작으면 그대로
#### ThresholdTypes.Tozero: 임계값보다 크면 그대로, 작으면 0
#### ThresholdTypes.TozeroInv: 임계값보다 크면 0, 작으면 그대로
#### ThresholdTypes.Ostu: 최적의 임계값 선택(thresholdValue는 무시됨!)
*내용평가: Tozero를 사용하면 임계값보다 크면 임계값 처리를 하는데 이러한 타입을 이용하여 크로마키와 같은 원하는 이미지 영역을 처리하는데 도움이 줄 수 있다고 생각*

## Cv2.AdaptiveThreshold(grayMat, dstMat, maxValue, Adaptive_method, type, blockSize, C);
#### Adaptive_method: 임계값을 결정하는 계산 방법
#### type: 임계점을 처리하는 방식
#### blockSize: 임계값을 적용할 영역의 크기
#### C: 평균이나 가중 평균에서 차감한 값

## ThresholdAdaptive Type → MeanC, GaussianC
#### AdaptiveThresholdTypes.MeanC: 주변영역의 평균값 - C로 결정
#### AdaptiveThresholdTypes.GaussianC: 주변영역의 가우시안 가중 평균값 - C로 결정
*내용평가: blocSize가 커질수록 주변 영역도 함께 maxValue로 처리가 됨, C값을 올리면 maxValue 처리된 부분이 0으로 바뀌는 보정이 들어가 깔끔하게 처리를 함. → Thresold 파라미터 영향을 받지 않음.*

Home Page 사용법: 콤보박스 Threshold 타입 설정 → 각 파라미터 슬라이드

# Blur&Kernel Page
##  Cv2.Blur(srcMat, dstMat, ksize);
#### Blur: 평균 필터를 기반으로 이미지를 부드럽게(흐리게) 변형하는 기법, 잡음(noise) 제거의 전처리 용도로 주로 사용

## Cv2.Dilate(srcMat, dstMat, Cv2.GetStructuringElement(shape, ksize));

#### Cv2.GetStructuringElement: 모폴리지 구조 커널 생성 함수
#### shape: 구조 요소 모양을 나타내는 플래그
#### ksize: 구조 요소 크기. (width, height) 튜플

## 모폴리지(Morphology): 영상을 형태학적인 측면으로 접근하는 것이며, 필터링과 비슷한 연산을 내부에서 진행
#### 1. 침식(erosion) 연산: 객체 외곽을 깍아내는 연산 → 객체(잡음) 제거 효과
#### 2. 팽창(dilation) 연산: 겍체 외곽을 팽창하는 연산 → 객체 크기는 감소되고 배경은 확대
*내용평기: 모폴리지에 dilate 함수로만 연산하여 필터링 하는 줄 알았기에 erode 관한 함수로도 적용 필요*

Blur&Kernel Page 사용법: 콤보박스 모폴리지 형태(도형) 설정 → 콤보박스 연산 타입 설정 → block size 슬라이드(블러는 모폴리지 제외 출력)

# Caliper Page
## 캘리퍼: 엣지 검출 도구
#### 1. 측정할 영역(ROI)을 설정
#### 2. ROI 안에서 한 방향으로 픽셀 밝기 변화를 검사
#### 3. 밝기가 급격하게 변하는 지점을 에지로 인식
#### 4. 검출된 에지를 이용해 위치, 거리, 폭 등을 계산

Caliper Page 사용법: X,Y,Width,Height 입력 후 Height 엔터(onchange)시 Roi 이미지 반영 → 스캔 방향 설정 → 캘리퍼 및 픽셀 범위 설정  → Roi 이미지 엣지부분 생성 → 좌표 데이터 생성

# LineFitting Page
## 라인피팅: 이미지에서 검출한 여러 점(에지 좌표 등)을 이용해 가장 잘 맞는 직선 계산 → 캘리퍼 응용

Caliper Page 사용법: X,Y,Width,Height 랜덤 → 라인검출 개수 설정 → 점, 직선 포인트 이미지 출력  → 좌표 데이터 생성