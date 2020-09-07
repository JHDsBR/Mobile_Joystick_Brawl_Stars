# Mobile_Joystick_Brawl_Stars
## Controle de movimentação para celular, similar ao do jogo "brawl stars". Implementação com Unity.


#### Como implementar:<br />
1. baixar e importar o arquivo [BrawlStarsJoystick.unitypackage](https://github.com/JHDsBR/Mobile_Joystick_Brawl_Stars/blob/master/BrawlStarsJoystick.unitypackage) ao seu projeto Unity.<br />
2. no Unity abra a basta que acabou de importar e adicione o prefab "Joystick" a sua cena<br />
3. clique no joystick e no script "BrawlStarsJoystick" adicione á variável "objectToMove" o gameObject que vai ser controlado<br />

#### Inspector:<br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Inspector](https://github.com/JHDsBR/Mobile_Joystick_Brawl_Stars/blob/master/Imagens/Inspector.png)<br /><br />

- **On Fullscreen**<br />
Se o controle pode ser usado em qualquer parte da tela<br /><br />
- **Object To Move**<br /> 
O que vai ser controlado<br /><br />
- **Rotate To Face**<br /> 
Manter 'Object To Move' virando para onde está olhando<br /><br />
- **Movement Speed**<br /> 
Velocidade com qual o 'Object To Move' se movimenta<br /><br />
- **Speed Sensitive**<br /> 
Quanto mais longe o seu dedo a partir do centro do joystick, mais rápido o 'Object To Move' se movimenta<br /><br />
- **Dead Area**<br /> 
Área central do joystick onde ele não reconhece movimento<br /><br />
- **Area Boundary**<br /> 
Distância que vai além do limite do joystick<br /><br />
- **Joystick Background**<br /> 
Parte maior do joystick, que fica na parte de trás<br /><br />
- **Joystick Foreground**<br /> 
Parte menor do joystick, que fica na parte da frente<br /><br />
- **Ignore By Collider**<br /> 
Outros objectos que podem causar interferência, por exemplo botões. Necessita que tenha Collider2D<br /><br />
- **Ignore By Name**<br /> 
Outros objectos que podem causar interferência, por exemplo botões. Necessita que tenha Collider2D<br /><br />

#### Observações:<br />
- Fiz testes apenas em um projeto 2D, não sei como pode se comportar em um ambiente 3D, acredito que você irá ter que fazer alterações por conta própria



