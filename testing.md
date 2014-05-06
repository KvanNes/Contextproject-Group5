UUnit gebruiken
===============

Voor testen gebruiken we [UUnit][https://github.com/pboechat/uunit/]. De code ervan komt in de repository.

Algemeen
--------

 - Test uitvoeren: In het menu: "UUnit -> Run All Tests".
 - Test maken: Maak een C# class. Inherit van UUnitTestCase. Voor de testfuncties in de class geldt:
    - Geef `void` return type.
    - Geef geen parameters.
    - Geef `[UUnitTest]` als annotation.

Documentatie
------------

Ik heb geen goede documentatie kunnen vinden, maar de code is zo klein dat je het zelf kunt snappen. In het bijzonder zijn er de volgende assert-functies:

 - `UUnitAssert.True(bool [, msg])`
 - `UUnitAssert.False(bool [, msg])`
 - `UUnitAssert.Fail()`
 - `UUnitAssert.NotNull(obj)`
 - `UUnitAssert.Null(obj)`
 - `UUnitAssert.Equals(obj1, obj2 [, msg])`

Voor `Equals` wordt voor `float`/`Vector3` een precisie gebruikt.

Voorbeeld test class
--------------------

  using UnityEngine;
  using System.Collections;

  public class Test : UUnitTestCase {
  
  	[UUnitTest]
  	public void Test1() {
  		UUnitAssert.True(true);
  	}
  }
