/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.h
  * @brief          : Header for main.c file.
  *                   This file contains the common defines of the application.
  ******************************************************************************
  * @attention
  *
  * Copyright (c) 2025 STMicroelectronics.
  * All rights reserved.
  *
  * This software is licensed under terms that can be found in the LICENSE file
  * in the root directory of this software component.
  * If no LICENSE file comes with this software, it is provided AS-IS.
  *
  ******************************************************************************
  */
/* USER CODE END Header */

/* Define to prevent recursive inclusion -------------------------------------*/
#ifndef __MAIN_H
#define __MAIN_H

#ifdef __cplusplus
extern "C" {
#endif

/* Includes ------------------------------------------------------------------*/
#include "stm32f4xx_hal.h"

/* Private includes ----------------------------------------------------------*/
/* USER CODE BEGIN Includes */

/* USER CODE END Includes */

/* Exported types ------------------------------------------------------------*/
/* USER CODE BEGIN ET */

/* USER CODE END ET */

/* Exported constants --------------------------------------------------------*/
/* USER CODE BEGIN EC */

/* USER CODE END EC */

/* Exported macro ------------------------------------------------------------*/
/* USER CODE BEGIN EM */

/* USER CODE END EM */

/* Exported functions prototypes ---------------------------------------------*/
void Error_Handler(void);

/* USER CODE BEGIN EFP */

/* USER CODE END EFP */

/* Private defines -----------------------------------------------------------*/
#define ADS1262_CS_Pin GPIO_PIN_1
#define ADS1262_CS_GPIO_Port GPIOA
#define ADS1262_RESET_Pin GPIO_PIN_4
#define ADS1262_RESET_GPIO_Port GPIOA
#define ADG1408_1_A0_Pin GPIO_PIN_0
#define ADG1408_1_A0_GPIO_Port GPIOB
#define ADG1408_1_A1_Pin GPIO_PIN_1
#define ADG1408_1_A1_GPIO_Port GPIOB
#define ADG1408_1_A2_Pin GPIO_PIN_2
#define ADG1408_1_A2_GPIO_Port GPIOB
#define ADS1262_DRDY_Pin GPIO_PIN_12
#define ADS1262_DRDY_GPIO_Port GPIOB
#define ADG1408_2_A0_Pin GPIO_PIN_13
#define ADG1408_2_A0_GPIO_Port GPIOB
#define ADS1262_START_Pin GPIO_PIN_8
#define ADS1262_START_GPIO_Port GPIOA
#define DAC8760_3_LATCH_Pin GPIO_PIN_5
#define DAC8760_3_LATCH_GPIO_Port GPIOB
#define ADG1408_2_A1_Pin GPIO_PIN_6
#define ADG1408_2_A1_GPIO_Port GPIOB
#define DAC8760_2_LATCH_Pin GPIO_PIN_7
#define DAC8760_2_LATCH_GPIO_Port GPIOB
#define ADG1408_2_A2_Pin GPIO_PIN_8
#define ADG1408_2_A2_GPIO_Port GPIOB
#define DAC8760_1_LATCH_Pin GPIO_PIN_9
#define DAC8760_1_LATCH_GPIO_Port GPIOB

/* USER CODE BEGIN Private defines */

/* USER CODE END Private defines */

#ifdef __cplusplus
}
#endif

#endif /* __MAIN_H */
