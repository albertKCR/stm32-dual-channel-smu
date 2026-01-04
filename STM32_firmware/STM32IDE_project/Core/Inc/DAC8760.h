#ifndef DAC8760_H
#define DAC8760_H

#include <stdint.h>
#include "usb_device.h"
#include "stm32f4xx_hal.h"

typedef struct {
    uint16_t latch_pin;
    GPIO_TypeDef *latch_port;
    SPI_HandleTypeDef hspi;
} DAC8760_t;

// Address Byte
#define ADDR_NOP        0x00 // No operation (NOP)
#define ADDR_DATA       0x01 // Write DAC data register
#define ADDR_READ       0x02 // Register read
#define ADDR_CTRL       0x55 // Write control register
#define ADDR_RESET      0x56 // Write reset register
#define ADDR_CONFIG     0x57 // Write configuration register
#define ADDR_GAIN       0x58 // Write DAC gain calibration register
#define ADDR_ZERO       0x59 // Write DAC zero calibration register
#define ADDR_WDT        0x95 // Watchdog timer reset

// Register Read Address Functions
#define READ_STATUS_REG     0x00
#define READ_DAC_DATA_REG   0x01
#define READ_CNTRL_REG      0x02
#define READ_CONFIG_REG     0x0B
#define READ_GAIN_CAL_REG   0x13
#define READ_ZERO_CAL_REG   0x17

void DAC8760_Init(DAC8760_t *dac, uint16_t latch, GPIO_TypeDef *port, SPI_HandleTypeDef hspi);
void DAC8760_WriteVoltage(DAC8760_t *dac, uint16_t value);
void DAC8760_WriteRegister(DAC8760_t *dac, uint8_t reg, uint16_t value);
uint16_t DAC8760_ReadRegister(DAC8760_t *dac, uint8_t reg);
void DAC8760_LATCH_LOW(DAC8760_t *dac);
void DAC8760_LATCH_HIGH(DAC8760_t *dac);

#endif
