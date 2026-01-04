#ifndef ADS1262_H
#define ADS1262_H

#include <stdint.h>
#include "usb_device.h"
#include "stm32f4xx_hal.h"

// Register Address
#define POWER       0x01
#define INTERFACE   0x02
#define MODE0       0x03
#define MODE1       0x04
#define MODE2       0x05
#define INPMUX      0x06
#define OFCAL0      0x07
#define OFCAL1      0x08
#define OFCAL2      0x09
#define FSCAL0      0x0A
#define FSCAL1      0x0B
#define FSCAL2      0x0C

// Commands
#define NOP         0x00
#define RESET       0x06
#define START       0x08
#define STOP        0x0A
#define RDATA       0x12

// PGA
#define PGA_GAIN_1  0x00
#define PGA_GAIN_2  0x10
#define PGA_GAIN_4  0x20
#define PGA_GAIN_8  0x30
#define PGA_GAIN_16 0x40
#define PGA_GAIN_32 0x50

// INPUT MUX
#define AIN0        0X00
#define AIN1        0x01
#define AIN2        0x02
#define AIN3        0x03
#define AIN4        0x04
#define AIN5        0x05
#define AIN6        0x06
#define AIN7        0x07
#define AIN8        0x08
#define AIN9        0x09
#define AINCOM      0x0A

extern SPI_HandleTypeDef hspi1;

void ADS1262_Init();
void ADS1262_Select();
void ADS1262_Deselect();
void ADS1262_START();
void ADS1262_HARD_STOP();
void HAL_GPIO_EXTI_Callback(uint16_t GPIO_Pin);
void ADS1262_WriteRegister(uint8_t reg, uint8_t value);
uint8_t ADS1262_ReadRegister(uint16_t reg);
uint32_t ADS1262_ReadData();
void ADS1262_setPGA(uint8_t PGA);
void ADS1262_setAIN(uint16_t AINP, uint16_t AINN);

#endif
