#include "DWIN.h"

void readVP(uint16_t vp, uint8_t numBytes)
{
    uint8_t cmd[] = {
        0x5A,
        0xA5,
        0x04,
        0x83,
        (vp >> 8) & 0xFF,
        vp & 0xFF,
        6,
    };

    HAL_UART_Transmit(&huart1, cmd, sizeof(cmd), HAL_MAX_DELAY);
}

void setDisplayPage(uint8_t pageId)
{
    uint8_t cmd[] = {
        0x5A,
        0xA5,
        0x07, // ID da MCU
        0x82, // Comando de leitura
        0x00,
        0x84,
        0x5A,
        0x01,
        0x00,
        pageId // ID da página
    };

    HAL_UART_Transmit(&huart1, cmd, sizeof(cmd), HAL_MAX_DELAY);
}

void readDisplayFloat(float *myFloat)
{
    uint8_t incomingByte[18] = {0};

    uint8_t buffer1[10];

    uint8_t receivedFloatArray[50];

    float receivedFloat;

    if (HAL_UART_Receive(&huart1, incomingByte, 18, 500) == HAL_OK)
    {

        if (incomingByte[4] == 0x83) // VP read instruction is in the returned char array
        {
            int k = 8;                      // starting point for the message - it is always a fixed distance
            int j = 0;                      // counter for the float array
            while (incomingByte[k] != 0xFF) // end character is  0xFFFF
            {
                receivedFloatArray[j] = incomingByte[k]; // Copy the contents to a new array (also notice the indexing!)
                k++;
                j++;
            }

            // convert the char array to float, then print the received number
            receivedFloat = atof(receivedFloatArray);
            *myFloat = receivedFloat;

            sprintf(buffer1, "%f", receivedFloat);
            CDC_Transmit_FS(buffer1, strlen(buffer1));

            // Finally, we empty the whole array to avoid garbage
            memset(incomingByte, 0, sizeof(incomingByte));             // Empty array
            memset(receivedFloatArray, 0, sizeof(receivedFloatArray)); // Empty array
        }
    }
}

char *getVP()
{
    uint8_t buffer1[10];

    uint8_t rxByte[6] = {0};
    if (HAL_UART_Receive(&huart1, rxByte, 6, 500) == HAL_OK)
    {
        sprintf(buffer1, "%02X%02X", rxByte[4], rxByte[5]);
        CDC_Transmit_FS(buffer1, strlen(buffer1));

        return buffer1;
    }
}

void dwinSendDouble(uint16_t vp, double doubleNumber)
{
    uint8_t hex[8] = {0};

    doubleToHex(doubleNumber, hex);

    uint8_t cmd[] = {
        0x5A,
        0xA5,
        0x0B,
        0x82,
        (vp >> 8) & 0xFF,
        vp & 0xFF,
		hex[7],
		hex[6],
		hex[5],
		hex[4],
		hex[3],
        hex[2],
		hex[1],
		hex[0]
    };

    HAL_UART_Transmit(&huart1, cmd, sizeof(cmd), HAL_MAX_DELAY);
}

void doubleToHex(double d, uint8_t *hex)
{
	memcpy(hex, &d, sizeof(double));
}
