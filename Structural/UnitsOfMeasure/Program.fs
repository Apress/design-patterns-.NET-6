

[<Measure>] type kg // kilograms
[<Measure>] type m  // meters
[<Measure>] type s  // seconds

[<Measure>] type N = kg m / s^2 // Newtons

let length = 3.0<m>
let time = 5.0<s>
let weight = 2.0<kg>

// will not compile
//let more_weight = 3<kg> + weight

let force: float<N> = (weight * length) / (time*time)
printf $"%f{force}" // 0.24