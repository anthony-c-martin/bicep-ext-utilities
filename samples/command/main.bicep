targetScope = 'local'

extension utilities

resource sayHello 'Command' = {
  command: 'gh auth status'
}

output stdout string? = sayHello.stdOut
