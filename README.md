# WebrtcSharp
Webrtc .Net API


  ```C#
    var factory = new PeerConnectionFactory();
    var configuration = new RTCConfiguration();
    configuration.AddServer("stun:stun.l.google.com:19302");
    var connection = factory.CreatePeerConnection(configuration);
    connection.IceCandidate += iceCandidate =>
    {
        connection.AddIceCandidate(iceCandidate);
    };
    var offer = await connection.CreateOffer();
    // any more...
  ```

Demo
  �״�����ʱ��Ҫ����node_modules
  ```
    cd rtcclient
    start npm install
    cd ..\rtcserver
    start npm install
  ```
  ����������������Ϳͻ��ˣ�
  ```
    cd rtcclient
    start npm test
    cd ..\rtcserver
    start npm test
  ```
  ������� http://localhost:8080/#/share/view/test-room
  Ȼ��򿪲����� ScreenShare\ScreenShare.csproj