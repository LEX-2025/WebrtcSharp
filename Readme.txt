һ��Ҫʹ��cmd��������powershell

��װpython2.7��������path

����depot_tools��
git clone https://chromium.googlesource.com/chromium/tools/depot_tools.git
depot_tools��PATH����Ҫ��python��ǰ�档��ע�⣬ϵͳpath���û�path��ǰ�棩
����б��ַ��������ҵ��ű���Ӧ��λ���޸��ַ������ɡ�

����ϵͳ����
set DEPOT_TOOLS_WIN_TOOLCHAIN=0

����ϵͳ������ַ�Ͷ˿ںŰ�ʵ�ʵ���
set http_proxy=127.0.0.1:53374
set https_proxy=127.0.0.1:53374
����git����
git config --global http.proxy 127.0.0.1:53374
git config --global https.proxy 127.0.0.1:53374

����git
git config --global user.name "my name"
git config --global user.email "my_email@address"
git config --global core.autocrlf false
git config --global core.filemode false
git config --global branch.autosetupmerge always
git config --global branch.autosetuprebase always
��git�����ļ���
git config --system core.longpaths true

������Դǰ�����У�
gclient

Ȼ������Դ�룺
mkdir webrtc
cd webrtc
fetch --nohooks webrtc
gclient sync

Ȼ����ȡĿ��汾��Ŀǰʹ��m76��
cd src
git checkout branch-heads/m76
git pull origin branch-heads/m76
gclient sync

����Release��Ŀ�ļ���
gn gen out/Default --args="is_debug=false target_cpu=\"x64\" is_clang=false rtc_include_tests=false" --ide=vs2017

����Release
ninja -C out/Default

�����Ҫ���ԣ���Ҫ����debug�汾

����DEBUG��Ŀ�ļ���
gn gen out/Debug --args="is_debug=true target_cpu=\"x64\" is_clang=false rtc_include_tests=false" --ide=vs2017

����Debug
ninja -C out/Debug