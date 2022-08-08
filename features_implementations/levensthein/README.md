# Levensthein Algorithm:

Understanding how optimize Levensthein Algorithm, following this one: 
https://github.com/Turnerj/Quickenshtein. 

Contiene 6 implementaciones, aparte de la más sencilla, cada una posee una optimización:

- v_1 reducir matriz completa a una sola fila.
- v_2 usar array pooling.
- v_3 eliminar comenzar común y final común.
- v_4 usar span() para optimizar respecto a Substring.
- v_5 optimizar para el caso cuando coincide las letras en dos índices
- v_6 evitar la creación de una variable varias veces, segundo for.

