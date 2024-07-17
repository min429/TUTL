public class User
{
    // 정적 변수로 인스턴스를 유지
    private static User instance;

    // 외부에서 접근할 수 있는 정적 메서드로 인스턴스에 접근
    public static User Instance
    {
        get
        {
            // 인스턴스가 null인 경우 생성
            if (instance == null)
            {
                instance = new User();
            }
            return instance;
        }
    }

    // 실제 유저 데이터
    public long id;
    public string username;
    public string accessToken;
    public string refreshToken;

    // 생성자는 private로 선언하여 외부에서 직접 인스턴스화하지 못하도록 함
    private User() {}

    // 기타 메서드나 속성 추가 가능
}
